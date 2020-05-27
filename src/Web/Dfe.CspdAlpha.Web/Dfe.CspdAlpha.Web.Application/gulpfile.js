'use strict';

const {src, dest, series, parallel} = require('gulp');
const sass = require('gulp-sass');
const del = require('del');
const run = require('gulp-run');
const sourcemaps = require('gulp-sourcemaps');
const postcss = require('gulp-postcss');
const autoprefixer = require('autoprefixer');
const cssnano = require('cssnano');
const gulpStylelint = require('gulp-stylelint');
//const eslint = require('gulp-eslint');

const scssPath = './Assets/Sass/*.scss';
const cssDest = './wwwroot/assets/stylesheets';

function cleanCss() {
  return del(cssDest);
}

function cleanJs(){
  return del('./wwwroot/assets/scripts/build');
}

function lintScss() {
  return src('./Assets/Sass/**/*.scss')
    .pipe(gulpStylelint({
      reporters: [
        {formatter: 'string', console: true}
      ]
    }));
}

// build compressed css file no source maps
function prodCss() {
  return src(scssPath)
    .pipe(sass())
    .pipe(postcss([autoprefixer(), cssnano()]))
    .pipe(dest(cssDest));
}

// build uncompressed css files with source maps
function devCss() {
  return src(scssPath)
    .pipe(sourcemaps.init())
    .pipe(sass())
    .pipe(postcss([autoprefixer()]))
    .pipe(sourcemaps.write('./sourcemaps'))
    .pipe(dest(cssDest));
}


function devJs() {
  return run('npm run compile-js:dev').exec();
}

function prodJs() {
  return run('npm run compile-js:prod').exec();
}

// function unitTest() {
//   return  run('npm test --verbose --ci').exec();
// }

const compileProdCss = series(cleanCss, lintScss,  parallel(prodCss));
const compileDevCss = series(cleanCss, lintScss, parallel(devCss));

const buildDevJs = series(cleanJs, devJs);
const buildProdJs = series(cleanJs, prodJs);

const buildDev = series(cleanCss, lintScss, parallel(devCss, buildDevJs));

const build = series(cleanCss, lintScss, parallel(prodCss, buildProdJs));

exports.compileCss = compileProdCss;
exports.compileDevCss = compileDevCss;

exports.buildJs = buildProdJs;
exports.buildDevJs = buildDevJs;

//exports.unitTest = unitTest;

exports.buildDev = buildDev;

exports.build = build;
