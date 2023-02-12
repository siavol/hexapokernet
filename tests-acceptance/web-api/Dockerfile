FROM node:18

WORKDIR /test

COPY ./package*.json ./
RUN npm ci

COPY . .

CMD ["npm", "test"]