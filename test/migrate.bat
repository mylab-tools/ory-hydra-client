@echo off
docker run -it --rm ^
  --network hydra-client-test ^
  oryd/hydra:v1.10.6 ^
  migrate sql --yes mysql://hydra:hydrapass@tcp(mysql:3306)/hydra?parseTime=true