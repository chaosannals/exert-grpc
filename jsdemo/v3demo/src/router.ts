import { kebabCase } from "lodash";
import {
    createRouter,
    createWebHistory,
    RouteRecordRaw,
} from "vue-router";

export function createMyDemoRouter() {
    const routes: Array<RouteRecordRaw> = [{
        path: '/',
        alias: '/index.html',
        component: () => import('./pages/Index.vue'),
    }, {
        path: '/:pathMatch(.*)*',
        component: () => import('./pages/NotFound.vue'),
    }];
    const pages = import.meta.glob('./pages/**/*.vue');
    for (const p of Object.keys(pages)) {
        const m = p.match(/.\/pages\/(.*)Page\.vue/);
        if (m) {
            const n = kebabCase(m!.at(1));
            routes.push({
                path: `/${n}`,
                alias: `/${n}.html`,
                component: pages[p],
            });
        }
    }
    return createRouter({
        history: createWebHistory(import.meta.env.BASE_URL),
        routes: routes,
    });
};