build:
	dotnet publish src/GothmogBot/GothmogBot.csproj -c Release -r linux-x64 -o ./output --sc true -p:DebugType=None -p:DebugSymbols=false --self-contained
	mv output/local.settings.prod.json output/local.settings.json

upload:
	rsync -av --no-perms --delete output/ philaeux@luna.the-cluster.org:/home/philaeux/gothmog-bot/output

deploy:
	git pull
	docker compose up --build -d
