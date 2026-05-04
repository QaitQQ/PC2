from django.apps import AppConfig

class YourAppConfig(AppConfig):
    name = 'WebApp'

    def ready(self):
        
        from .startup import startup_routine
        startup_routine()