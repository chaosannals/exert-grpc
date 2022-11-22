import { kebabCase } from "lodash";
import Vue from 'vue';
import VueRouter from 'vue-router';

class Router extends VueRouter {
    constructor(options) {
        super(options);
        this.options = options;
    }

    /**
     * 重置。
     * 
     */
    reset() {
        let other = new VueRouter(this.options);
        this.matcher = other.matcher;
    }

    /**
     * 捕获同个路由导致的异常。
     * 
     * @param {*} location 
     * @param {*} resolve 
     * @param {*} reject 
     */
    push(location) {
        return super.push(location).catch(e => {
            if (e && e.name != "NavigationDuplicated" && e.message.indexOf('redundant') < 0) {
                Promise.reject(e);
            }
        });
    }
}

Vue.use(Router);

function parseRoutes() {
    const requireComponent = require.context(
        '@/pages/',
        true,
        /.+?Page\.vue$/,
    );
    const routes = [
        {
            path: '/',
            name: 'index',
            alias: '/index.html',
            component: ()=>import('@/pages/IndexPage.vue'),
        }
    ];
    requireComponent.keys().forEach(p => {
        const component = requireComponent(p);
        const m = p.match(/.\/(.+?)Page.vue$/);
        if (m) {
            const n = kebabCase(m.at(1));
            routes.push({
                path: `/${n}`,
                name: `${n}-page`,
                alias: `/${n}.html`,
                component: component.default || component,
            });
            console.log(`${p} => ${n}`);
        }
    });
    return routes;
}

export const router = new Router({
    mode: 'history',
    base: '/',
    routes: parseRoutes(),
});
