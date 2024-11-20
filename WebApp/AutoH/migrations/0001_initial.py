# Generated by Django 4.2.2 on 2024-05-24 08:21

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    initial = True

    dependencies = [
    ]

    operations = [
        migrations.CreateModel(
            name='Device',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('sn', models.CharField(max_length=25, null=True, verbose_name='oldValue')),
            ],
        ),
        migrations.CreateModel(
            name='DeviceType',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('name', models.CharField(max_length=25, null=True, verbose_name='oldValue')),
            ],
        ),
        migrations.CreateModel(
            name='DeviceData',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('data', models.CharField(max_length=200, verbose_name='Data')),
                ('device', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='AutoH.device', verbose_name='Device')),
            ],
        ),
        migrations.AddField(
            model_name='device',
            name='deviceType',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='AutoH.devicetype', verbose_name='DeviceType'),
        ),
    ]
