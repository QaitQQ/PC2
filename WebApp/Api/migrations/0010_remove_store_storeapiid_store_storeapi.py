# Generated by Django 4.2.2 on 2023-08-16 10:34

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('Api', '0009_store_alter_items_order_orders_store'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='store',
            name='storeApiId',
        ),
        migrations.AddField(
            model_name='store',
            name='storeApi',
            field=models.CharField(max_length=50, null=True, unique=True, verbose_name='ID'),
        ),
    ]
