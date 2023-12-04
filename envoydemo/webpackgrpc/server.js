const Koa = require('koa');
const app = new Koa();
const serve = require('koa-static');

app.use(serve('public'));

app.listen(30000);
