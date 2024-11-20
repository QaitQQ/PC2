from os import name
from django.db import models

class Device(models.Model):
    sn= models.CharField(verbose_name="oldValue", max_length=25, null=True)  
    deviceType = models.ForeignKey("DeviceType", verbose_name='DeviceType', on_delete=models.CASCADE, null=True)
    
class DeviceType(models.Model):
    name = models.CharField(verbose_name="oldValue", max_length=25, null=True)
    
class DeviceData(models.Model):
    device = models.ForeignKey("Device", verbose_name='Device', on_delete=models.CASCADE)
    data =  models.CharField(verbose_name="Data", max_length=200, null=True)
    
class DeviceStatus(models.Model):
    device = models.ForeignKey("Device", verbose_name='Device', on_delete=models.CASCADE)
    data =  models.DateTimeField(verbose_name="Date", max_length=200, null=True)