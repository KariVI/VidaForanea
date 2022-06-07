from email.policy import default
from models.Comment import Comment

from pkg_resources import require
from marshmallow import Schema, fields, validate





class CommentSchema(Schema):
    class Meta:
       model: Comment
    id = fields.Int(dump_only=True)
    student = fields.String(require=True)
    date =fields.String(required=True)
    hour =fields.String(required=True, validate=[validate.Length(max=5)])
    description = fields.String(required=True)
    forum_id = fields.Int(require=True)