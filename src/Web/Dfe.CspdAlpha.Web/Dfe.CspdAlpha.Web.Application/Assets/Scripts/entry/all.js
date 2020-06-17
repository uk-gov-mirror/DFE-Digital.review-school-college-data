/**
 * Modules that are instantiated on every page
 */

import {initAll} from 'govuk-frontend';
import accordionExtensions from '../AppModules/GovUkComponentExtensions/appAccordionExtensions';
import AppRibbonNavigation from '../AppModules/Navigation/AppRibbonNavigation';
import AppPrint from '../AppModules/AppPrint';
initAll();

accordionExtensions();

if (document.getElementById('app-ribbon-nav')) {
  const ribbonNav = new AppRibbonNavigation();
  let resizeTimer;

  $(window).on('resize', function() {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(function() {
      ribbonNav.reinit();

    }, 750);
  });
}

AppPrint();
