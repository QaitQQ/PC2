# Generated by Django 4.2.2 on 2023-08-01 07:10

import datetime
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('main', '0006_alter_atable_anons_alter_atable_date_and_more'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='atable',
            options={},
        ),
        migrations.AlterModelOptions(
            name='btable',
            options={},
        ),
        migrations.AlterField(
            model_name='atable',
            name='date',
            field=models.DateTimeField(null=datetime.date(2023, 8, 1), verbose_name='Дата'),
        ),
    ]
