const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:36348';

const context =  [
    "/weatherforecast",
    "/weatherforecast/ping",
    "/searchdata",
    "/getFavoriteRestaurants",
    "/getreviewedrestaurants",
    "/favoriterestaurant/get",
    "/favoriterestaurant/add",
    "/favoriterestaurant/delete",
    "/UserFavoriteRestaurant/add",
    "/UserFavoriteRestaurant/delete",
    "/visitedrestaurantlist/add",
    "/visitedrestaurantlist/delete",
    "/visitedrestaurantlist/get",
    "/reviewrestaurant/add",
    "/reviewrestaurant/get",
    "/reviewrestaurant/list",
    "/api/auth"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
