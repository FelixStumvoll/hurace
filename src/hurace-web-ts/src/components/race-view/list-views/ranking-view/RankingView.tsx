import React, { useState, useEffect } from 'react';
import { RaceRanking } from '../../../../interfaces/RaceRanking';
import { setStateAsync } from '../../../../common/stateSetter';
import { getRankingForRace } from '../../../../api';
import { ListHost } from '../../ListHost';
import { RankingViewItem } from './RankingViewItem';

export const RankingView: React.FC<{ raceId: number }> = ({ raceId }) => {
    const [raceRanking, setRaceRanking] = useState<RaceRanking[] | undefined>(
        undefined
    );

    useEffect(() => {
        if (raceRanking !== undefined) return;
        setStateAsync(setRaceRanking, getRankingForRace(raceId));
    }, [raceRanking, raceId]);

    return (
        <ListHost headerText="Rangliste">
            {raceRanking?.map(rr => (
                <RankingViewItem raceRanking={rr} />
            ))}
        </ListHost>
    );
};
