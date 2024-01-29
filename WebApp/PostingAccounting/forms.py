from dataclasses import field
from pydoc import plainpager
from PostingAccounting.models import Package, Item
from django import forms


class ItemForm(forms.ModelForm):
    class Meta:
        model = Item
        fields = ["sku", "count", "price", "url", "description"]
        widgets = {
            "sku": forms.Textarea(
                attrs={
                    "style": "height:calc(1.5em + .75rem + 2px);width:100%;",
                    
                    "placeholder": "Текст",
                    "type": "CharField",
                }
            ),
            "url": forms.Textarea(
                attrs={
                  "style": "height:calc(1.5em + .75rem + 2px);width:100%;",
                    
                    "placeholder": "url",
                    "type": "URLField",
                }
            ),
            "description": forms.Textarea(
                attrs={
                  "style": "height:calc(1.5em + .75rem + 2px);width:100%;",
                
               
                    "placeholder": "Дата Распоковки",
                    "type": "TextField",
                }
            ),
            "count": forms.Textarea(
                attrs={
                   "style": "height:calc(1.5em + .75rem + 2px);width:100%;",
                    
               
                    "placeholder": "Дата Распоковки",
                    "type": "TextField",
                }
            ),
            "price": forms.Textarea(
                attrs={
                  "style": "height:calc(1.5em + .75rem + 2px);width:100%;",
                  
                }
            ),
        }


class PackageForm(forms.ModelForm):
    def __init__(self, *args, **kwargs):
        super(PackageForm, self).__init__(*args, **kwargs)
        self.orderId = self.instance.id

        self.orderItems = []
        Items = Item.objects.filter(order=self.orderId)
        for X in Items:
            self.orderItems.append(ItemForm(instance=X))

    orderItems = []
    orderId = 0

    itemForm = ItemForm()

    class Meta:
        model = Package
        fields = ["orderNomber", "orderDate", "unboxingDate"]
        widgets = {
            "orderNomber": forms.Textarea(
                attrs={
                    "style": "height:calc(1.5em + .75rem + 2px);width:100%;",
                    "class": "form-control",
                    "placeholder": "Текст",
                }
            ),
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
