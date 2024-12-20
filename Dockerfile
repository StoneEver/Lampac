FROM node:16-alpine
WORKDIR /app
COPY . .
RUN npm install
CMD ["node", "main.js"]
EXPOSE 8080
