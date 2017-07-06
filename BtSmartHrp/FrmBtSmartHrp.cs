using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Devices.Enumeration.Pnp;
using Windows.Storage.Streams;

namespace BtSmartHrp
{
    public struct BtSmartDeviceInfo {
        public string Name;
        public DeviceInformation DeviceInfo;
    }

    public partial class FrmBtSmartHrp : Form
    {
        private DeviceInformationCollection btSmartDevices;
        private string deviceContainerId;
        private GattDeviceService service;
        private GattCharacteristic characteristic;
        private PnpObjectWatcher watcher;
        private bool running = false;

        private const byte HEART_RATE_VALUE_FORMAT = 0x01;

        public FrmBtSmartHrp()
        {
            InitializeComponent();
        }

        private async void FrmBtSmartHrp_Load(object sender, EventArgs e)
        {
            btSmartDevices = await DeviceInformation.FindAllAsync(
                GattDeviceService.GetDeviceSelectorFromUuid(GattServiceUuids.HeartRate),
                new string[] { "System.Devices.ContainerId" }
            );

            foreach (DeviceInformation btSmartDevice in btSmartDevices)
            {
                cbDevice.Items.Add(new BtSmartDeviceInfo { Name = btSmartDevice.Name, DeviceInfo = btSmartDevice });
            }

            if (cbDevice.Items.Count > 0)
                cbDevice.SelectedIndex = 0;
        }

        private async void btnStartStop_Click(object sender, EventArgs e)
        {
            if (!running)
                await Start();
            else
                Stop();

            if (running)
            {
                btnStartStop.Text = "Stop";
                cbDevice.Enabled = false;
                nudCharacteristic.Enabled = false;
                nudInitDelay.Enabled = false;
            }
            else
            {
                btnStartStop.Text = "Start";
                cbDevice.Enabled = true;
                nudCharacteristic.Enabled = true;
                nudInitDelay.Enabled = true;
                lbBpm.Text = "--";
            }
        }

        private void Log(string text) {
            tbLog.AppendText(text + Environment.NewLine);
        }

        private async Task Start()
        {
            try
            {
                DeviceInformation device = ((BtSmartDeviceInfo)cbDevice.SelectedItem).DeviceInfo;
                deviceContainerId = "{" + device.Properties["System.Devices.ContainerId"] + "}";
                int characteristicIndex = Decimal.ToInt32(nudCharacteristic.Value);
                int initDelay = Decimal.ToInt32(nudInitDelay.Value);

                Log("Getting GattDeviceService " + device.Name + " with id " + device.Id);

                GattDeviceService service = await GattDeviceService.FromIdAsync(device.Id);
                if (initDelay > 0)
                    await Task.Delay(initDelay);

                // Obtain the characteristic for which notifications are to be received
                Log("Getting HeartRateMeasurement GattCharacteristic " + characteristicIndex);
                characteristic = service.GetCharacteristics(GattCharacteristicUuids.HeartRateMeasurement)[characteristicIndex];

                // While encryption is not required by all devices, if encryption is supported by the device,
                // it can be enabled by setting the ProtectionLevel property of the Characteristic object.
                // All subsequent operations on the characteristic will work over an encrypted link.
                Log("Setting EncryptionRequired protection level on GattCharacteristic");
                characteristic.ProtectionLevel = GattProtectionLevel.EncryptionRequired;

                // Register the event handler for receiving notifications
                if (initDelay > 0)
                    await Task.Delay(initDelay);

                Log("Registering event handler onction level on GattCharacteristic");
                characteristic.ValueChanged += characteristic_ValueChanged;

                // In order to avoid unnecessary communication with the device, determine if the device is already 
                // correctly configured to send notifications.
                // By default ReadClientCharacteristicConfigurationDescriptorAsync will attempt to get the current
                // value from the system cache and communication with the device is not typically required.
                Log("Reading GattCharacteristic configuration descriptor");
                var currentDescriptorValue = await characteristic.ReadClientCharacteristicConfigurationDescriptorAsync();

                if ((currentDescriptorValue.Status != GattCommunicationStatus.Success) ||
                    (currentDescriptorValue.ClientCharacteristicConfigurationDescriptor != GattClientCharacteristicConfigurationDescriptorValue.Notify))
                {
                    // Set the Client Characteristic Configuration Descriptor to enable the device to send notifications
                    // when the Characteristic value changes
                    Log("Setting GattCharacteristic configuration descriptor to enable notifications");

                    GattCommunicationStatus status =
                        await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                        GattClientCharacteristicConfigurationDescriptorValue.Notify);

                    if (status == GattCommunicationStatus.Unreachable)
                    {
                        Log("Device unreachable");

                        // Register a PnpObjectWatcher to detect when a connection to the device is established,
                        // such that the application can retry device configuration.
                        StartDeviceConnectionWatcher();
                    }
                }
                else
                {
                    Log("Configuration successfull");
                }

                running = true;
            }
            catch (Exception e)
            {
                Log("Error configuring HRP device " + e);
            }
        }

        private void StartDeviceConnectionWatcher()
        {
            watcher = PnpObject.CreateWatcher(PnpObjectType.DeviceContainer,
                new string[] { "System.Devices.Connected" }, String.Empty);

            Log("Registering device connection watcher updated event handler");
            watcher.Updated += DeviceConnection_Updated;

            Log("Starting device connection watcher");
            watcher.Start();
        }

        private async void DeviceConnection_Updated(PnpObjectWatcher sender, PnpObjectUpdate args)
        {
            Log("Device connection updated, args = " + args);

            var connectedProperty = args.Properties["System.Devices.Connected"];
            bool isConnected = false;
            if ((deviceContainerId == args.Id) && Boolean.TryParse(connectedProperty.ToString(), out isConnected) &&
                isConnected)
            {
                var status = await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                    GattClientCharacteristicConfigurationDescriptorValue.Notify);

                if (status == GattCommunicationStatus.Success)
                {
                    Log("Stopping device connection watcher");

                    // Once the Client Characteristic Configuration Descriptor is set, the watcher is no longer required
                    watcher.Stop();
                    watcher = null;
                    Log("Configuration successfull");
                }
            }
        }

        void characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            Log("GattCharacteristic value changed, args = " + args);
            byte[] data = new byte[args.CharacteristicValue.Length];

            DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(data);

            ProcessData(data, args.Timestamp);
        }

        private void ProcessData(byte[] data, DateTimeOffset timestamp)
        {
            Log("Processing HRP payload, data = " + data);

            byte currentOffset = 0;
            byte flags = data[currentOffset];
            bool isHeartRateValueSizeLong = ((flags & HEART_RATE_VALUE_FORMAT) != 0);

            currentOffset++;

            ushort heartRateMeasurementValue = 0;

            if (isHeartRateValueSizeLong)
            {
                heartRateMeasurementValue = (ushort)((data[currentOffset + 1] << 8) + data[currentOffset]);
                currentOffset += 2;
            }
            else
            {
                heartRateMeasurementValue = data[currentOffset];
                currentOffset++;
            }

            lbBpm.Text = heartRateMeasurementValue.ToString();

            Log("HR reading = " + heartRateMeasurementValue);
        }

        private void Stop()
        {
            Log("Stopping HRP");

            if (characteristic != null)
            {
                Log("Clearing GattCharacteristic");
                characteristic.ValueChanged -= characteristic_ValueChanged;
                characteristic = null;
            }

            if (watcher != null)
            {
                Log("Clearing device changed watcher");
                watcher.Stop();
                watcher = null;
            }

            if (service != null)
            {
                Log("Clearing GattDeviceService");
                service.Dispose();
                service = null;
            }

            running = false;
        }
    }
}
