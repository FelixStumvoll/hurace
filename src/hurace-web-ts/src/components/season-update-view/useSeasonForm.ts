import { useState, useEffect } from 'react';

import { getSeasonById } from '../../common/api';

export const useSeasonForm = (
    seasonId?: number
): [SeasonFormValues | undefined] => {
    const [initialFormValue, setInitialFormValue] = useState<
        SeasonFormValues
    >();

    useEffect(() => {
        const loadData = async () => {
            if (!seasonId) {
                setInitialFormValue({
                    startDate: new Date(),
                    endDate: new Date()
                });
            } else {
                let season = await getSeasonById(seasonId);
                setInitialFormValue({
                    startDate: season.startDate,
                    endDate: season.endDate
                });
            }
        };

        loadData();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return [initialFormValue];
};
