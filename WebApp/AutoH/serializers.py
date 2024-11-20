from collections import OrderedDict
from django.db.models import QuerySet, query
from rest_framework import serializers
from rest_framework.fields import ValidationError

from AutoH.models import *


class DeviceSerializer(serializers.ModelSerializer):
    class Meta:
        model = Device
        validators=[]
        fields =    '__all__'
        
class DataSerializer(serializers.ModelSerializer):
    class Meta:
        model = DeviceData
        validators=[]
        fields =    '__all__'
        
class SingleDataSerializer(serializers.ModelSerializer):
    class Meta:
        model = DeviceData
        validators=[]
        fields =    ['data',]
