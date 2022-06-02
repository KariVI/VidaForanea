from email.policy import default
from marshmallow import Schema, fields

from utils import hash_password


class StudentSchema(Schema):
    class Meta:
        ordered = True
    
    id = fields.Int(dump_only=True)
    name = fields.String(required=True)   
    enrollment =fields.String(required=True)
    password = fields.Method(required=True, deserialize='load_password')
    status = fields.Boolean(default=True)
    rol = fields.String( default='estudiante')
    degree = fields.String(required=True)


    def load_password(self, value):
        return hash_password(value)