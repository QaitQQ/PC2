# Generated by Django 4.2.2 on 2023-08-15 10:20

import datetime
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('main', '0009_alter_atable_date'),
    ]

    operations = [
        migrations.AlterField(
            model_name='atable',
            name='date',
            field=models.DateTimeField(null=datetime.date(2023, 8, 15), verbose_name='Дата'),
        ),
    ]