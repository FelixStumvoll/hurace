import React from 'react';
import { RaceRanking } from '../../../../interfaces/RaceRanking';
import { getTimeString } from '../../../../common/timeConverter';

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
                    : getTimeString(raceRanking.time!)}
            </td>
            <td>
                {raceRanking.disqualified
                    ? '-'
                    : getTimeString(raceRanking.timeToLeader!)}
            </td>
        </tr>
    );
};
