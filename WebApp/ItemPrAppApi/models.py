from tabnanny import verbose
from django.db import models
from datetime import date
from django.contrib.auth import get_user_model
User = get_user_model()
class Change(models.Model):
    itemChange = models.ForeignKey("Item", verbose_name='Item', on_delete=models.CASCADE)
    fieldСhange = models.ForeignKey("FieldСhange", verbose_name='FieldСhange', on_delete=models.CASCADE)
    oldValue = models.CharField(verbose_name="Sku", max_length=25, null=True)    
    newValue = models.CharField(verbose_name="Sku", max_length=25, null=True) 
    source = models.ForeignKey("Sourse", verbose_name='Sourse', on_delete=models.CASCADE)
    dateСhange = models.DateTimeField(verbose_name="DateСhange")
    user = models.ForeignKey(User, verbose_name= 'Пользователь', on_delete=models.CASCADE, null=True)
class Item(models.Model):
    name = models.CharField(verbose_name="Name", max_length=200)
    sku = models.CharField(verbose_name="Sku", max_length=25, null=True)
    description = models.ForeignKey("ItemDescription",verbose_name='Description', on_delete=models.CASCADE, null=True)
    changes = models.ForeignKey(Change,verbose_name='Changes', on_delete=models.CASCADE, null=True)
    categories = models.ForeignKey("Categories",verbose_name='Categories', on_delete=models.CASCADE, null=True)
    price = models.FloatField(verbose_name="Price")
    manufactor = models.ForeignKey("Manufactor",verbose_name='Manufactor', on_delete=models.CASCADE, null=True)
    details = models.ManyToOneRel("Details", field_name='Details',  on_delete=models.CASCADE, to='item')
    images = models.ManyToOneRel("Image", field_name='Images',  on_delete=models.CASCADE, to='item')
    itemComparisonName = models.ManyToOneRel("ComparisonName", field_name='itemComparisonName',  on_delete=models.CASCADE, to='item')
    stocks = models.ManyToOneRel("Stock", field_name='Stocks',  on_delete=models.CASCADE, to='item')
    def get_absolute_url(self):
        return f'/{self.id}'
class ItemDescription(models.Model):
    itemDesc = models.ForeignKey(Item, verbose_name='Item', on_delete=models.CASCADE)
    description = models.CharField(verbose_name="Description", max_length=250)
    descriptionSeparator = models.CharField(verbose_name="DescriptionSeparator", max_length=10)
class FieldСhange(models.Model):
    name = models.CharField(verbose_name="Name", max_length=50) 
class Sourse(models.Model):
    name = models.CharField(verbose_name="Name", max_length=50) 
class Manufactor(models.Model):
    name = models.CharField(verbose_name="Name", max_length=50)
    param = models.CharField(verbose_name="Param", max_length=200)
class Stock(models.Model):
    item = models.ForeignKey(Item, verbose_name='Item', related_name = "stocks", on_delete=models.CASCADE,null=True)
    warehouse = models.ForeignKey("Warehouse", verbose_name='Warehouse', on_delete=models.CASCADE,null=True)
    count = models.IntegerField(verbose_name="Count")
class Warehouse(models.Model):
    name = models.CharField(verbose_name="Name", max_length=50) 
class Categories(models.Model):
    parent = models.ForeignKey("Categories", verbose_name='Parent', on_delete=models.CASCADE,null=True)
    name = models.CharField(verbose_name="Name", max_length=50) 
class Details(models.Model):
    item = models.ForeignKey(Item, verbose_name='Item', related_name = "details", on_delete=models.CASCADE,null=True)
    name = models.CharField(verbose_name="Name", max_length=50) 
    description = models.CharField(verbose_name="Description", max_length=250)
class Image(models.Model):
    item = models.ForeignKey(Item, verbose_name='Item', related_name = "images", on_delete=models.CASCADE,null=True)
    uri = models.CharField(verbose_name="Name", max_length=50) 
class ComparisonName(models.Model):
    item = models.ForeignKey(Item, verbose_name='Item', related_name = 'itemComparisonName', on_delete=models.CASCADE, null=True)
    cname = models.CharField(verbose_name="Name", max_length=50, null=True) 
    def __str__(self):
        return self.id
    
