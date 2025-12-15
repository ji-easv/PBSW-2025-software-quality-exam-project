import {test, expect} from "@playwright/test";

test('Box list page renders correctly', async ({page}) => {
  await page.goto('/');
  await page.getByRole('link', {name: 'Box List'}).click();

  await expect(page.getByRole('heading', {name: 'List of all boxes'})).toBeVisible();
  await expect(page.getByRole('button', {name: 'ADD NEW BOX'})).toBeVisible();
  await expect(page.getByRole('table')).toBeVisible();
});
