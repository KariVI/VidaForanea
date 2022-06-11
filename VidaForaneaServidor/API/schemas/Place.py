from email.policy import default
from models.Comment import Comment

from pkg_resources import require
from marshmallow import Schema, fields, validate, ValidationError


class BytesField(fields.Field):
    def _validate(self, value):
        if not isinstance(value, bytes):
            raise ValidationError('Invalid input type.')

        if value is None or value == b'':
            raise ValidationError('Invalid value')


class PlaceSchema(Schema):
    class Meta:
       model: Comment
    id = fields.Int(dump_only=True)
    name = fields.String(require=True)
    address =fields.String(required=True)
    services =fields.String(required=True)
    schedule = fields.String(required=True)
    type_place = fields.String(required=True)
    image = BytesField()
    status = fields.String(required=True)

