from email.policy import default
from models.Comment import Comment

from pkg_resources import require
from marshmallow import Schema, fields, validate
from models.Opinion import Opinion





class OpinionSchema(Schema):
    class Meta:
       model: Opinion
    id = fields.Int(dump_only=True)
    student = fields.String(require=True)
    date =fields.String(required=True)
    hour =fields.String(required=True, validate=[validate.Length(max=5)])
    description = fields.String(required=True)
    id_place = fields.Int(require=True)