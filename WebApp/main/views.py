from os import error
from django.shortcuts import render, redirect
from .forms import AtableForm
from main.models import Atable
from django.views.generic import DetailView, UpdateView, DeleteView

def index(request):
    if request.method == "GET":
       data = {"title": "Переданные данные", "values": [
        #"один", 1, 2.4
        ]}
       return render(request, "main/index.html", data)
    else:
     return render(request, "main/index.html")


def One(request):
    tables = Atable.objects.order_by("-date")  # [:2]
    return render(request, "main/One.html", {"tables": tables})

class ListDetailView(DetailView):
    model= Atable
    template_name='main/ListDetailView.html'
    context_object_name='list'

class DetailUp(UpdateView):
    model= Atable
    template_name='main/add.html'

    form_class=AtableForm
    #fields=['title','full_text','date' ]

class DetailDel(DeleteView):
    model= Atable
    template_name='main/del.html'
    success_url='/One'

def add(request):
    error = ""
    if request.method == "POST":
        form = AtableForm(request.POST)
        if form.is_valid():
            form.save()
            return redirect("home")
        else:
            error = "Ошибка"
    form = AtableForm()
    data = {"form": form, "error": error}
    return render(request, "main/add.html", data)



