import {test, expect} from "@playwright/test";
import {captureApiRequests} from "./utils/test-helper";

test('Box list page renders correctly', async ({page}) => {
  const requests = captureApiRequests(page);
  await page.goto('/');
  await page.getByRole('link', {name: 'Box List'}).click();

  await expect(page).toHaveURL(/.*\/boxlist/);
  await expect(page.getByRole('heading', {name: 'List of all boxes'})).toBeVisible();
  await expect(page.getByRole('button', {name: 'ADD NEW BOX'})).toBeVisible();
  await expect(page.getByRole('table')).toBeVisible();

  const boxRequest = requests.find(req => req.url().includes('/box?currentPage=1&boxesPerPage=10'));
  expect(boxRequest).toBeDefined();
  const boxResponse = await boxRequest!.response();
  expect(boxResponse).not.toBeNull();
  if (boxResponse) {
    expect(boxResponse.status()).toBe(200);
    const responseBody = await boxResponse.json();
    expect(responseBody).toHaveProperty('boxes');
    expect(Array.isArray(responseBody.boxes)).toBe(true);
  }
});

test('Box search functionality works', async ({page}) => {
  const requests = captureApiRequests(page);
  await page.goto('/boxlist');

  const searchInput = page.getByPlaceholder('Search');
  await searchInput.fill('Red');
  await page.getByRole('button', {name: 'Search'}).click();

  const searchRequest = requests.find(req => req.url().includes('/box?currentPage=1&boxesPerPage=10&searchTerm=Red'));
  expect(searchRequest).toBeDefined();
  const searchResponse = await searchRequest!.response();
  expect(searchResponse).not.toBeNull();

  if (searchResponse) {
    expect(searchResponse.status()).toBe(200);
    const responseBody = await searchResponse.json();
    expect(responseBody).toHaveProperty('boxes');
    expect(Array.isArray(responseBody.boxes)).toBe(true);
    responseBody.boxes.forEach((box: any) => {
      expect(box.name).toContain('Sample Box');
    });
  }
});
