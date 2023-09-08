from curses.ascii import isdigit
from dataclasses import fields
from email.policy import default
from enum import unique
from os import name, read
from pyexpat import model
import re
from turtle import mode
from urllib import request
from wsgiref.validate import validator
from rest_framework import serializers
from rest_framework.fields import ValidationError
from ItemPrAppApi.models import Change, Item, ItemDescription, FieldСhange, Sourse, Manufactor, Stock, Warehouse, Categories, Details, Image, ComparisonName
class ImageSerializer(serializers.ModelSerializer):
    class Meta:
        model = Image
        validators=[]
        fields =    '__all__'
class DetailsSerializer(serializers.ModelSerializer):
    class Meta:
        model = Details
        validators=[]
        fields =   '__all__'
class ItemDescriptionSerializer(serializers.ModelSerializer):
    class Meta: 
        model = ItemDescription
        validators=[]
        fields =  ['description','descriptionSeparator']
class ComparisonNameSerializer(serializers.ModelSerializer):
    class Meta:
        model = ComparisonName
        validators=[]
        fields =   ["cname"] 
class ItemsSerializer(serializers.ModelSerializer):
    description = ItemDescriptionSerializer(read_only=True)
    itemComparisonName = ComparisonNameSerializer(read_only = True, many=True) 
    class Meta:
        model = Item
        fields =  '__all__'
    def CreateItem(data):        
        descriptionBooble = False
        detailBooble = False
        imagesBooble  = False
        comparisonNameBooble = False
        if 'description' in data:
            descriptionBooble = data.pop('description')  
        if 'itemComparisonName' in data:           
            comparisonNameBooble = data.pop('itemComparisonName') 
        if 'images' in data:
            imagesBooble = data.pop('images') 
        if 'detail' in data:
            detailBooble = data.pop('detail') 
        item = Item.objects.create(**data)
        if descriptionBooble:       
            description = ItemDescription.objects.create(itemDesc=item, **descriptionBooble)
            item.description = description
        if comparisonNameBooble:
            comparisonNames = list()
            for cname in comparisonNameBooble:               
                 comparisonName = ComparisonName.objects.create(item=item,**cname)
                 comparisonNames.append(comparisonName)       
        else:
            comparisonNames = list()
            ccname = "";
            for ch in item.name:              
                if (ch.isalnum() and not ch.isspace() and len(ccname)<15):
                    ccname=ccname+ch
            ccname = ccname.lower()
            tcname = ccname        
            i = 1
            while ComparisonName.objects.filter(cname = tcname).exists():
                print(tcname)
                tcname =  ccname+"v"+str(i)
                i=i+1
            comparisonName = ComparisonName.objects.create(item=item, cname=tcname)
            comparisonNames.append(comparisonName);
        item.itemComparisonName.set(comparisonNames);
        if imagesBooble:
            images = list()
            for image in imagesBooble:               
                 imageB = ImageSerializer.objects.create(item=item,**image)
                 images.append(imageB)       
            item.images = images
        if detailBooble:
            details = list()
            for detail in detailBooble:               
                 detailB = ComparisonName.objects.create(item=item,**detail)
                 details.append(detailB)       
            item.details = details
        item.save()
        return item
    def create(self, validated_data):
        return self.CreateItem(self.initial_data)
class ItemsListCreateSerializer(serializers.Serializer):
    Items = list(ItemsSerializer())
    class Meta:
        fields = ["Items"]
    def create(self, validated_data):
        items = list()
        if 'Items' in self.initial_data:
            itemsBooble = self.initial_data.pop('Items')             
            for x in itemsBooble: 
               # print(str(x))
                items.append(ItemsSerializer.CreateItem(x))           
            return items
        else:
            raise ValidationError("Нет поля Items")
class ItemCashDictSerializer(serializers.ModelSerializer):
    itemComparisonName = ComparisonNameSerializer(read_only = True,many=True)
    class Meta:
        model = Item
        fields = ["id","name","itemComparisonName"]
        
class ClearBaseSerializer(serializers.Serializer):
    
    def delete():
        print("123") 
        item = Item.objects.all().delete()       


