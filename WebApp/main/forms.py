from dataclasses import field
from pydoc import plainpager
from tkinter import Widget
from .models import Atable
from django.forms import ModelForm, TextInput, Textarea, DateTimeInput


class AtableForm(ModelForm):
    class Meta:
        model = Atable
        fields = ["title", "full_text", "date"]

        widgets = {
            "title": TextInput(
                attrs={
                    "class": "form-control",
                    "placeholder": "Название",
                }
            ),
            "full_text": Textarea(
                attrs={
                    "class": "form-control",
                    "placeholder": "Текст",
                }
            ),
            "date": DateTimeInput(
                attrs={"class": "form-control", "placeholder": "Дата", "type": "data"}
            ),
        }