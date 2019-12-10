var path = require("path");
var webpack = require("webpack");

module.exports = {
    entry: {
        guessgame: [
            './ClientApp/client-start.js',
            'bootstrap/dist/css/bootstrap.css',
            'bootstrap/dist/css/bootstrap-theme.css',
            './ClientApp/styles.css'
        ]
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: "jquery",
            jQuery: "jquery"
        })
    ],
    output: {
        path: path.resolve(__dirname, 'wwwroot'),
        filename: '[name].js',
        publicPath: '/'
    },
    module: {
        rules: [
            { test: /\.js/, include: path.resolve(__dirname, 'ClientApp'), use: { loader: 'babel-loader', options: { presets: ['env'] } } },
            { test: /\.css$/, use: ['style-loader', 'css-loader'] },
            { test: /\.(png|jpg|jpeg|gif|woff|woff2|eot|ttf|svg)$/, use: 'url-loader?limit=25000' }
        ]
    }
};