'use strict';

const webpack = require('webpack');
const path = require('path');
//const CleanWebpackPlugin = require('clean-webpack-plugin');
const glob = require('glob');
//const VueLoaderPlugin = require('vue-loader/lib/plugin');


const entryDirPath = path.resolve('./Assets/Scripts/entry/');
const entryFiles = path.join(entryDirPath, '**/*.js');
const buildDir = path.resolve('./wwwroot/assets/scripts/build');

module.exports = {
  // Creates a mapping of all files in `entry` directory
  // e.g {'all': 'path/to/all.js'}
  entry: glob
    .sync(entryFiles)
    .reduce((acc, filePath) => {
      const file = path.parse(filePath);
      acc[file.name] = path.resolve(process.cwd(), filePath);
      return acc;
    }, {}),

  optimization: {
    splitChunks: {
      cacheGroups: {
        vendor: {
          chunks: 'initial',
          test: 'site-common.bundle.[chunkhash].js',
          name: 'site-common.bundle.[chunkhash].js',
          enforce: true
        }
      }
    }
  },

  resolve: {
    alias: {
      modules: path.resolve('./Assets/Scripts/modules'),
      helpers: path.resolve('./Assets/Scripts/helpers'),
      //'vue$': 'vue/dist/vue.esm.js'
    },
    extensions: ['*', '.js', /*'.vue',*/ '.json']
  },


  output: {
    path: buildDir,
    filename: '[name].bundle.[chunkhash].js',
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        loader: 'babel-loader',
      },
      //{
      //  test: /\.vue$/,
      //  loader: 'vue-loader'
      //}
    ],
  },

  plugins: [
    //new VueLoaderPlugin(),
    new webpack.EnvironmentPlugin([
      'NODE_ENV',
    ]),
    new webpack.ProvidePlugin({
      $: 'jquery',
      jQuery: 'jquery',
    }),

    // Clean out the build directory between each build
    //new CleanWebpackPlugin(buildDir, {root: process.cwd()}),

  ],
};
