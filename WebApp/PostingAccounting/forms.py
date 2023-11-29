from dataclasses import field
from pydoc import plainpager
from PostingAccounting.models import Package, Item
from django import forms


class ItemForm(forms.ModelForm):
    class Meta:
        model = Item
        fields = '__all__'





class PackageForm(forms.ModelForm):
    orderItems = forms.ModelMultipleChoiceField(queryset=Item.objects.all())
    name = forms.CharField();
    class Meta:
        model = Package
       
        fields = '__all__'

        widgets = {
            "orderNomber": forms.Textarea(
                attrs={
                    "class": "form-control",
                    "placeholder": "Текст",
                }
            ),
            
            "orderItems": forms.ModelMultipleChoiceField(queryset=Item.objects.all() ),

            "orderDate": forms.DateTimeInput(
                attrs={
                    "class": "form-control",
                    "placeholder": "Дата Заказа",
                    "type": "data",
                }
            ),
            "unboxingDate": forms.DateTimeInput(
                attrs={
                    "class": "form-control",
                    "placeholder": "Дата Распоковки",
                    "type": "data",
                }
            ),
        }
        
    
    