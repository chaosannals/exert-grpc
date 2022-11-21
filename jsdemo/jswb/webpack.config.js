const path = require('path');

module.exports = {
    mode: 'production',
    entry: {
        jswb: path.resolve(__dirname, 'src', 'main.ts'),
        igrpc: path.resolve(__dirname, 'src', 'igrpc.ts'),
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, '..', 'v3demo', 'public')
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
        ],
    },
    resolve: {
        extensions: ['.ts', '.js'],
    },
};