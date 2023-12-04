const path = require('path');

module.exports = {
  mode: "production",
  entry: {
    main:"./main.js"
  },
  output: {
    filename: "[name].js",
    path: path.resolve(__dirname, "public"),
  },
};
