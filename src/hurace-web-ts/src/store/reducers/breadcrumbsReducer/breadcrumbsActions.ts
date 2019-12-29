import {
    BreadcrumbsAction,
    BreadcrumbsActionType
} from './breadcrumbsActionType';

export const addBreadcrumb = (
    name: string,
    url: string
): BreadcrumbsAction => ({
    type: BreadcrumbsActionType.Add,
    breadcrumb: { name, url }
});

export const clearBreadcrumbs = (): BreadcrumbsAction => ({
    type: BreadcrumbsActionType.Clear
});
