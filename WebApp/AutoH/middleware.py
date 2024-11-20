
from django.utils.deprecation import MiddlewareMixin



class RemoveHeaders(MiddlewareMixin):
      
    def process_response(self, request, response):
        response['Server'] = ''
        return response