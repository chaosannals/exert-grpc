import { kebabCase } from "lodash";

export default {
    install(Vue) {
        const requireComponent = require.context(
            '@/components/',
            true,
            /.+?\.vue$/,
        );
        requireComponent.keys().forEach(p => {
            console.log(p);
            const component = requireComponent(p);
            const m = p.match(/.\/(.+?).vue$/);
            if (m) {
                const n = kebabCase(m.at(1));
                Vue.component(
                    n,
                    component.default || component,
                )
            }
        });
    }
}