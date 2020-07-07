package com.example.dwdwproject.repositories.devicesRepositories;

import android.content.Context;

import com.example.dwdwproject.ResponseDTOs.DeviceDTO;
import com.example.dwdwproject.models.Device;
import com.example.dwdwproject.utils.CallBackData;

import java.util.List;
public interface DeviceRepositories {
    void getAllDeviceList(Context mContext,String token, CallBackData<List<DeviceDTO>>mCallBackData);
    void getAllDeviceById(Context mContext,String token,int deviceId,CallBackData<DeviceDTO>callBackData);
    void createDevice(Context mContext,String token,DeviceDTO mDevice,CallBackData<DeviceDTO> callBackData);
    void updateDevice(Context mContext,String token,DeviceDTO mDevice,CallBackData<DeviceDTO> callBackData);
    void getAllDeviceFromLocationByAdmin(Context mContext,String token,int locationId,CallBackData<List<DeviceDTO>>mCallBackData);
    void getAllDeviceFromLocationByManager(Context mContext,String token,int locationId,CallBackData<List<DeviceDTO>>mCallBackData);
}
