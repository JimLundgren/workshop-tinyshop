# TinyShop Product Navigation + Detail Page Plan

## Problem and approach
Add product navigation in the Store web UI so selecting a product from `/products` navigates to a new detail page at `/products/{id}`. The detail page will load product data via the existing `ProductService.GetProduct(int id)` API call, render image/name/price/description, and show a friendly not-found message when the id is invalid or missing in the API.

This will be implemented with minimal, surgical changes in the Store Razor components and aligned tests in `TinyShop.Tests` for changed logic (service behavior and related functionality) while preserving existing UI behavior and styling conventions.

## Clarified requirements (from user)
- Route pattern: `/products/{id}`
- Detail page fields: image, name, price, description
- Invalid/missing product: friendly not-found message
- Product list interaction: whole product card clickable
- Include test updates in same implementation

## Todos
1. **Review and confirm integration points**
	- Verify product list component structure, available service methods, and route handling in Store.
	- Confirm image URL behavior uses `Configuration["ImagePrefix"]`.

2. **Create product detail page component**
	- Add new Razor page in `Store\Components\Pages` with route `/products/{id:int}`.
	- Inject `ProductService` and `IConfiguration`, fetch product on initialize, and implement loading/success/not-found states.
	- Render required fields (image, name, price, description) and ensure image path uses `ImagePrefix`.

3. **Enable clickable navigation from product list**
	- Update product list cards in `Products.razor` so the whole card navigates to `/products/{id}`.
	- Preserve existing list layout and content, adding minimal semantic/accessible markup for clickable cards.

4. **Apply simple inline styling where needed**
	- Keep UI simple with inline CSS in affected Razor files.
	- Add only minimal styles required for clickable-card behavior and detail-page readability.

5. **Add/update tests**
	- Extend `TinyShop.Tests` for behavior touched by this change (likely service-focused tests around single-product retrieval outcomes and edge handling).
	- Add project reference(s) only if required for test compilation and keep scope tight to changed behavior.

6. **Validate implementation**
	- Run targeted build/test commands for impacted projects.
	- Verify no regressions in existing product list behavior and that new navigation/detail flow works as specified.

## Notes and considerations
- Reuse existing `ProductService.GetProduct(int id)` endpoint contract (`/api/Product/{id}`) and null-on-failure behavior.
- Keep error handling explicit in UI states (loading, not found, found) without silent fallbacks.
- Avoid unrelated refactors; only touch files needed for navigation + detail experience + tests.