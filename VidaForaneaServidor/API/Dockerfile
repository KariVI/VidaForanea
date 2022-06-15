FROM python:3.8-slim-buster

EXPOSE 9000/tcp
EXPOSE 9000/udp
WORKDIR ./VidaForaneaServidor/API
COPY ./VidaForaneaServidor/API/requirements.txt ./VidaForaneaServidor/API/requirements.txt
RUN pip3 install -r requirements.txt

COPY . .

CMD [ "python3", "-m" , "flask", "run", "--host=0.0.0.0"]