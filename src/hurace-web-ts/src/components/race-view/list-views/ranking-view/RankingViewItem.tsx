import React from 'react';
import { RaceRanking } from '../../../../interfaces/RaceRanking';
import { getSkierTimeString } from '../../../../common/timeConverter';

export const RankingViewItem: React.FC<{ raceRanking: RaceRanking }> = ({
    raceRanking
}) => {
    return (
        <tr>
            <td>{raceRanking.position}</td>
            <td>{raceRanking.startList.startNumber}</td>
            <td>{raceRanking.startList.skier.country.countryCode}</td>
            <td>
                {raceRanking.startList.skier.firstName}{' '}
                {raceRanking.startList.skier.lastName}
            </td>
            <td>
                {raceRanking.disqualified
                    ? '-'
                    : getSkierTimeString(raceRanking.time!)}
            </td>
            <td>
                {raceRanking.disqualified
                    ? '-'
                    : getSkierTimeString(raceRanking.timeToLeader!)}
            </td>
        </tr>
    );
};
