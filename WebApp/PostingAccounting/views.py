from ast import Import
from dataclasses import fields
from itertools import count
from django.shortcuts import render, redirect
from rest_framework.permissions import IsAuthenticated, IsAdminUser, AllowAny
from django.views.generic import DetailView, UpdateView, DeleteView
from PostingAccounting.models import Package, Item
from rest_framework.parsers import JSONParser
from PostingAccounting.forms import PackageForm, ItemForm
from django.views.decorators.csrf import requires_csrf_token, csrf_exempt
from rest_framework.decorators import api_view, permission_classes
from django.http import QueryDict


def index(request):
    if request.method == "GET":
        tables = Package.objects.order_by("-orderDate")  # [:2]
        data = {"title": "Посылки", "tables": tables}
        return render(request, "PostingAccounting/index.html", data)
    else:
        return render(request, "PostingAccounting/index.html")


@csrf_exempt
def ItemAdd(request):
    if request.method == "POST":
        nItem = QueryDict(request.POST["item"]).copy()
        nOrder = Package.objects.filter(id=request.POST["order"])
        nItem.pop("csrfmiddlewaretoken")
        count = nItem.pop("count")
        newItem = Item.objects.create(
            order=nOrder[0],
            count=int(count[0]),
            sku=nItem.pop("sku")[0],
            price=nItem.pop("price")[0],
            url=nItem.pop("url")[0],
            description=nItem.pop("description")[0],
        )
        newItem.save()
    return render(request, "main/Modal.html")


class PackageDetailView(DetailView):
    model = Package
    template_name = "PostingAccounting/DetailView.html"
    form = PackageForm()
    context_object_name = "list"


def PackageAdd(request):
    error = ""
    if request.method == "POST":
        form = PackageForm(request.POST)
        fields = "__all__"
        if form.is_valid():
            print("Все правильно")
            form.save()
            return redirect("PostingAccountingMainPage")
        else:
            error = "Ошибка"
    form = PackageForm()
    data = {"form": form, "error": error}
    return render(request, "PostingAccounting/new_posting.html", data)


# def ItemAdd(request):
#     error = ""
#     if request.method == "GET":
#         form = ItemForm(request.GET)
#         fields = '__all__'
#         if form.is_valid():
#             form.save()
#             return redirect("PostingAccountingMainPage")
#         else:
#             error = "Ошибка"
#     form = ItemForm()
#     data = {"form": form, "error": error}
#     return render(request, "PostingAccounting/new_item.html", data)
class PackageUpdate(UpdateView):
    model = Package
    template_name = "PostingAccounting/new_posting.html"
    form_class = PackageForm


class PackageRemove(DeleteView):
    model = Package
    template_name = "PostingAccounting/del.html"
    success_url = "../"
