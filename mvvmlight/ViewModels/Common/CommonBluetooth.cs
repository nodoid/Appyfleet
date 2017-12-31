using System;
using System.Collections.Generic;
using System.Linq;

namespace mvvmframework.ViewModels.Common
{
    public class CommonBluetooth : BaseViewModel
    {
        VehicleListModel registeredVehicles;
        public VehicleListModel RegisteredVehicles
        {
            get { return registeredVehicles; }
            set
            {
                Set(() => RegisteredVehicles, ref registeredVehicles, value, true);
                if (value != null)
                {
                    if (value.Vehicles != null)
                        VehicleModels = value.Vehicles;
                }
            }
        }

        VehicleModel selectedVehicle;
        public VehicleModel SelectedVehicle
        {
            get { return selectedVehicle; }
            set { Set(() => SelectedVehicle, ref selectedVehicle, value, true); }
        }

        Guid selectedBluetoothDevice;
        public Guid SelectedBluetoothDevice
        {
            get { return selectedBluetoothDevice; }
            set { Set(() => SelectedBluetoothDevice, ref selectedBluetoothDevice, value, true); }
        }

        string vehicleSearch;
        public string VehicleSearch
        {
            get { return vehicleSearch; }
            set { Set(() => VehicleSearch, ref vehicleSearch, value); }
        }

        List<BluetoothDevice> bluetoothDeviceList;
        public List<BluetoothDevice> BluetoothDeviceList
        {
            get { return bluetoothDeviceList; }
            set { Set(() => BluetoothDeviceList, ref bluetoothDeviceList, value, true); }
        }

        public bool SearchBasedOnVehicle()
        {
            if (string.IsNullOrEmpty(VehicleSearch))
                return false;

            if (RegisteredVehicles == null)
                return false;

            var vehicle = new List<VehicleModel>();
            vehicle = RegisteredVehicles?.Vehicles?.Where(t => t.Nickname.ToLowerInvariant().Equals(VehicleSearch.ToLowerInvariant())).ToList();
            if (vehicle.Count == 0)
            {
                vehicle = RegisteredVehicles?.Vehicles?.Where(t => t.Make.ToLowerInvariant().Equals(VehicleSearch.ToLowerInvariant())).ToList();
                if (vehicle.Count == 0)
                    return false;
            }

            VehicleModels = vehicle;
            return true;
        }

        public void SelectVehicle(string id)
        {
            var vehicle = RegisteredVehicles.Vehicles?.FirstOrDefault(t => t.BluetoothId.Equals(id));
            if (vehicle != null)
            {
                SelectedVehicle = vehicle;
                SelectedBluetoothDevice = new Guid(vehicle.BluetoothId);
                BluetoothId = bluetoothId;
                Make = vehicle.Make;
                Model = vehicle.Model;
                Nickname = vehicle.Nickname;
                Registration = vehicle.Registration;
                PairingId = vehicle.PairingId;
                Paired = vehicle.Paired;
            }
        }

        string bluetoothId;
        public string BluetoothId
        {
            get { return bluetoothId; }
            set { Set(() => BluetoothId, ref bluetoothId, value, true); }
        }

        public long Id { get; set; }

        string make;
        public string Make
        {
            get { return make; }
            set { Set(() => Make, ref make, value, true); }
        }

        string model;
        public string Model
        {
            get { return model; }
            set { Set(() => Model, ref model, value, true); }
        }

        string nickname;
        public string Nickname
        {
            get { return nickname; }
            set { Set(() => Nickname, ref nickname, value, true); }
        }

        string odometer;
        public string Odometer
        {
            get => odometer;
            set { Set(() => Odometer, ref odometer, value, true); }
        }

        public bool Paired { get; set; }
        public long PairingId { get; set; }

        string registration;
        public string Registration
        {
            get { return registration; }
            set { Set(() => Registration, ref registration, value, true); }
        }
    }
}
