from marshmallow import Schema, fields

from utils import hash_password


class ForumSchema(Schema):
    class Meta:
        ordered = True
    id = fields.Int(dump_only=True)
    degree = fields.String(required=True)