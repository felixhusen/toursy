import { localtourTemplatePage } from './app.po';

describe('localtour App', function() {
  let page: localtourTemplatePage;

  beforeEach(() => {
    page = new localtourTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
