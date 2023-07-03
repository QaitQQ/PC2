# Generated by Django 4.2.2 on 2023-06-29 13:38

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('main', '0004_alter_atable_options'),
    ]

    operations = [
        migrations.CreateModel(
            name='Btable',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('title', models.CharField(max_length=50, verbose_name='Название')),
                ('anons', models.CharField(max_length=250, verbose_name='Анонс')),
                ('full_text', models.TextField(verbose_name='Текст')),
                ('date', models.DateTimeField(verbose_name='Дата')),
            ],
            options={
                'verbose_name': 'ТаблицаB',
                'verbose_name_plural': 'ТаблицыB',
            },
        ),
        migrations.AlterModelOptions(
            name='atable',
            options={'verbose_name': 'ТаблицаА', 'verbose_name_plural': 'ТаблицыА'},
        ),
    ]
