# Generated by Django 4.2.2 on 2023-08-09 12:47

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('Api', '0007_orders_orderitems_alter_items_order'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='orders',
            name='orderItems',
        ),
    ]