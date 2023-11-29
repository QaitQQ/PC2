from ast import Import
from dataclasses import fields
from django.shortcuts import render, redirect
from django.views.generic import DetailView, UpdateView, DeleteView
from PostingAccounting.models import Package, Item 
from PostingAccounting.forms import PackageForm, ItemForm


def index(request):
    if request.method == "GET":
       tables = Package.objects.order_by("-orderDate")  # [:2]
       data = {
            'title': "Посылки", 
            'tables': tables}
       
       
       return render(request, "PostingAccounting/index.html",data )
    else:
     return render(request, "PostingAccounting/index.html")


def ItemAdd(request):

    return render(request, "main/Modal.html")

class PackageDetailView(DetailView):
    model= Package
    template_name='PostingAccounting/DetailView.html'
    form = PackageForm()
    context_object_name='list'
    

def PackageAdd(request):
    error = ""
    if request.method == "POST":
        form = PackageForm(request.POST)
        fields = '__all__'
        if form.is_valid():
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
    model= Package
    template_name='PostingAccounting/new_posting.html'
    form_class=PackageForm


class PackageRemove(DeleteView):
    model= Package
    template_name='PostingAccounting/del.html'
    success_url=''
    
