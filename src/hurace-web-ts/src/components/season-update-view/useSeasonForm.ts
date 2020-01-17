import { useState, useEffect } from 'react';

import { getSeasonById } from '../../common/api';
import { useCallState } from '../../hooks/useCallState';
import { ApiState } from '../../interfaces/ApiState';

export const useSeasonForm = (
    seasonId?: number
): [ApiState, SeasonFormValues | undefined] => {
    const [initialFormValue, setInitialFormValue] = useState<
        SeasonFormValues
    >();

    const callState = useCallState();

    useEffect(() => {
        const loadData = async () => {
            callState.setError(undefined);
            callState.setLoading(true);

            try {
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
            } catch (error) {
                callState.setError(error);
            } finally {
                callState.setLoading(false);
            }
        };

        loadData();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return [callState, initialFormValue];
};
