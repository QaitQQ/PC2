# Generated by Django 4.2.2 on 2023-11-24 09:30

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('PostingAccounting', '0001_initial'),
    ]

    operations = [
        migrations.AddField(
            model_name='item',
            name='description',
            field=models.CharField(max_length=500, null=True, verbose_name='Описание'),
        ),
        migrations.AddField(
            model_name='item',
            name='url',
            field=models.CharField(max_length=500, null=True, verbose_name='ССЫЛКА'),
        ),
        migrations.AlterField(
            model_name='item',
            name='sku',
            field=models.CharField(max_length=50, verbose_name='SKU'),
        ),
    ]