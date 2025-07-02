import { createRootRoute, Link, Outlet } from '@tanstack/react-router';
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools';

export const Route = createRootRoute({
	component: () => (
		<div className='bg-orange-75 dark:bg-zinc-850 text-zinc-900 dark:text-zinc-300
    break-words leading-6 transition-colors duration-500'>
			<div className='p-2 flex gap-2'>
				<Link to='/' className='[&.active]:font-bold'>
					Home
				</Link>{' '}
				<Link to='/about' className='[&.active]:font-bold'>
					Admin
				</Link>
			</div>
			<hr />
      <div className='mx-auto py-4 px-4 flex flex-col gap-8 items-start justify-center'>
			  <Outlet />
      </div>
			<TanStackRouterDevtools />
		</div>
	),
});
